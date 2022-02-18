import cv2
import numpy
import packages.opencv.Drawtools as Drawtools
import packages.opencv.FrameObjectSingle as FrameO
import packages.math.mathFunctions as mathFunctions


class TrackerO:
    def __init__(self):
        self.tag = None
        self.tracker = None
        self.init = False
        self.trackOk = None
        self.trackBox = None
        self.rootBox = None

        # Attempt to handle tracker tracking black area
        self.trackFrame = FrameO.FrameObjectSingle("trackFrame")

        # Attempt to combat track loss
        self.trackBoxArray = numpy.array([])
        self.extrapolatedBox = None
        self.extrapolationCount = 0
        self.extrapolationCountTotal = 0
        self.columns = 3
        self.rows = 4

    def ops(self, param, *objects):
        if param == "init tracker":
            if not self.init:
                self.tracker = cv2.TrackerCSRT_create()
                self.trackOk = self.tracker.init(objects[0], tuple(self.trackBox))
                self.init = True
        if param == "set tracker tag":
            self.tag = objects[0]
        if param == "set tracker tld":
            self.tracker = cv2.TrackerTLD_create()
        if param == "set tracker mosse":
            self.tracker = cv2.TrackerMOSSE_create()
        if param == "set tracker csrt":
            self.tracker = cv2.TrackerCSRT_create()
            # self.tracker = cv2.TrackerMedianFlow_create()
            # self.tracker = cv2.TrackerBoosting_create() Promising - needs color tuning to eliminate skin
        if param == "update tracker clear":
            self.tracker = None
        if param == "set tracker trackbox":
            self.trackBox = objects[0]
        if param == "get tracker trackbox":
            return self.trackBox
        if param == "set tracker rootbox":
            self.rootBox = objects[0]
        if param == "set tracker trackbox clear":
            self.trackBox = None
        if param == "update tracker":
            self.trackOk, tempBox = self.tracker.update(objects[0])
            self.trackBox = numpy.asarray(tempBox)

            if len(self.trackBoxArray) >= ((self.rows - 1) * self.columns):
                if (self.trackBoxArray[-4] - self.trackBoxArray[-8]) != 0:
                    self.extrapolatedBox = numpy.array(
                        mathFunctions.Extrapolate(self.trackBoxArray,
                                                  (int((len(self.trackBoxArray) / self.rows)), self.rows)))
                    Drawtools.drawBox(objects[0], self.extrapolatedBox, (152, 245, 255))

            if self.trackBox[0] < 0 or self.trackBox[1] < 0 or self.trackBox[0] > 499 or self.trackBox[1] > 499:
                Drawtools.drawMultipleText(objects[0],
                                           ("Box out of bounds", "Attempting retrack..."),
                                           numpy.array([100, 380]), (0, 0, 255))
                self.tracker = None
                self.init = False
                self.trackBox = self.rootBox
                self.trackBoxArray = numpy.array([])
                self.extrapolationCount = 0

            elif not self.trackOk:
                Drawtools.drawMultipleText(objects[0],
                                           ("Tracking Failure", "Rest or extrapolate..."),
                                           numpy.array([100, 380]), (0, 0, 255))
                if self.extrapolationCount < 3:
                    self.extrapolationCount = self.extrapolationCount + 1
                    self.extrapolationCountTotal = self.extrapolationCountTotal + 1
                    self.trackBox = self.extrapolatedBox
                else:
                    self.trackBox = self.rootBox
                    self.extrapolationCount = 0
                self.tracker = None
                self.init = False

                # Combat black box tracking part
                # trackBoxRegion = objects[0]
                # cv2.imshow("trackBoxRegion" , trackBoxRegion)
                # self.trackFrame.ops("set input frame", )
                # ...

            else:
                # Success
                # add latest found array point for future extrapolation
                self.extrapolationCount = 0
                if len(self.trackBoxArray) < (self.rows * self.columns):
                    self.trackBoxArray = numpy.append(self.trackBoxArray, self.trackBox).astype(int)
                else:
                    self.trackBoxArray = numpy.delete(self.trackBoxArray, [0, 1, 2, 3])
                    self.trackBoxArray = numpy.append(self.trackBoxArray, self.trackBox).astype(int)

                Drawtools.drawBox(objects[0], self.trackBox, (0, 255, 0))
                Drawtools.drawMultipleText(objects[0], ("trackBox",
                                                        "X: " + str(int(self.trackBox[0])),
                                                        "Xw: " + str(int(self.trackBox[0]) + int(self.trackBox[2])),
                                                        "Y: " + str(int(self.trackBox[1])),
                                                        "Yh: " + str(
                                                            int(self.trackBox[1]) + int(self.trackBox[3]))),
                                           numpy.array([100, 380]),
                                           (0, 255, 0))
                Drawtools.drawText(objects[0], ("Extrapolations: " + str(self.extrapolationCountTotal)),
                                   numpy.array([300, 380]), (0, 255, 0))
