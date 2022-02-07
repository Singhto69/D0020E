import cv2
import numpy as np
import packages.opencv.Drawtools as Drawtools
import packages.opencv.Masks as Masks
import packages.general.Functions as Functions
import packages.opencv.TrackerO as TrackerO
import packages.network.SocketO as SocketO

# StartX , StartY , Width ( from StartX to ... ) , Height ( from StartY to ...)

# define a video capture object
vid = cv2.VideoCapture(0)
ret, frame = vid.read()

# initialize tracker


# For tracker... pip install opencv-contrib-python==3.4.11.45
#trackObj = TrackerO.TrackerO()
#trackObj.setTrackBox(np.array([287, 150, 130, 130]))
#trackObj.setTrackBox((287,150,130,130))
#trackObj.setCSRT()

tracker = cv2.TrackerCSRT_create()
trackBox = np.array([287, 150, 130, 130])
trackInit = False

sock = SocketO.SocketO()
sock.setIP("192.168.10.166")
sock.setPort(5065)
sock.setUDP()
sock.createSocket()



while True:
    timer = cv2.getTickCount()
    # Capture the video frame by frame
    ret, frame = vid.read()
    inverted = cv2.flip(frame, 1)

    # The black region in the mask has the value of 0,
    # so when multiplied with original image removes all non-blue regions
    result = cv2.bitwise_and(frame, frame, mask=Masks.maskMergedGen(frame,cv2.COLOR_BGR2HSV))
    #result = Masks.maskGen(frame,cv2.COLOR_BGR2HSV)

    # Tracker
    if not trackInit:
        trackOk = tracker.init(result, tuple(trackBox))
        trackInit = True

#    if not trackObj.init:
#        trackOk = trackObj.tracker.init(result, trackObj.trackBox)
#        trackObj.init = True

    trackOk, trackBox = tracker.update(result)
    trackBox = np.asarray(trackBox)

    if trackOk:
        Drawtools.drawBox(result, trackBox, (0, 255, 0))
        Drawtools.drawMultipleText(result, ("trackBox",
                                            "X: " + str(int(trackBox[0])),
                                            "Xw: " + str(int(trackBox[0]) + int(trackBox[2])),
                                            "Y: " + str(int(trackBox[1])),
                                            "Yh: " + str(int(trackBox[1]) + int(trackBox[3]))), np.array([100, 380]),
                                   (0, 255, 0))
        sock.socket.sendto((Functions.boxCordsToString(trackBox,"Seb")).encode(), (sock.ip, sock.port))

    else:
        # Tracking failure
        Drawtools.drawText(result, "Tracking failure detected", np.array([100, 380]), (0, 0, 255))
        #trackObj.tracker.update(result)

    # Calculate and display fps
    # fps = cv2.getTickFrequency()/(cv2.getTickCount()- timer)
    # cv2.putText(result,str(fps),(75,50),cv2.FONT_HERSHEY_COMPLEX,0.7,(255,255,255),2)

    # Display options
    # cv2.imshow('frame', frame)
    cv2.imshow('inverted', inverted)
    # cv2.imshow('mask', mask)
    cv2.imshow('result', result)

    # break video capture
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# After the loop release the cap object
vid.release()
cv2.destroyAllWindows()
