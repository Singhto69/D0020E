import cv2

class TrackerO:
    def __init__ (self):
        self.tracker = None
        self.init = False
        self.trackOk = None
        self.trackBox = None

    def setTrackBox(self,array):
        self.trackBox = array
    def setCSRT(self):
        self.tracker = cv2.TrackerTLD_create()
    def setMOSSE(self):
        self.tracker = cv2.TrackerMOSSE_create()
    def setTLD(self):
        self.tracker = cv2.TrackerCSRT_create()




