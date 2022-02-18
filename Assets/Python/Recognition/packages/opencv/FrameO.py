import cv2
import packages.opencv.Masks as Masks


class FrameO:
    def __init__(self, tag):
        self.frames = None
        self.ret = None
        self.tag = tag
        self.inputSource = None
        self.dummy = None
        self.inputOn = False

    def ops(self, param, *objects):
        if param == "return tag":
            return self.tag
        if param == "set tag":
            self.tag = objects[0]
        if param == "show frame":
            cv2.imshow(self.tag, self.frame)
        if param == "close frame":
            cv2.destroyWindow(self.frame)
        if param == "set frame -inv":
            self.frame = cv2.flip(self.inputSource, 1)
        if param == "set frame -input":
            self.ret, self.frame = self.inputSource.read()
        if param == "return frame":
            return self.frame
        if param == "set input -cam":
            self.inputSource = cv2.VideoCapture(0)
        if param == "set input -frame":
            self.inputSource = objects[0]
        if param == "set frame -mergedmaskbitand":
            # The black region in the mask has the value of 0,
            # so when multiplied with original image removes all non-blue regions
            self.frame = cv2.bitwise_and(self.inputSource, self.inputSource,
                                         mask=Masks.maskMergedGen(self.inputSource,
                                                                  cv2.COLOR_BGR2HSV,
                                                                  objects[0],
                                                                  objects[1]))
