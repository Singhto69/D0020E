import cv2
import numpy as np
import Functions
import math
import socket

# StartX , StartY , Width ( from X to ) , Height ( from Y to )
trackBox = (287, 100, 80, 200)
collideBox = (10, 100, 50, 50)

# define a video capture object
vid = cv2.VideoCapture(0)
ret , frame = vid.read()

# initialize tracker
#tracker = cv2.TrackerMOSSE_create()
tracker = cv2.TrackerCSRT_create()
trackinit = False

#bbox = cv2.selectROI(frame, False)
#pip install opencv-contrib-python==3.4.11.45

#UDP_IP = "127.0.0.1"
UDP_IP = "192.168.10.207"
UDP_PORT = 5065
#AF_INET = IPv4
#SOCK_DGRAM = UDP
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

while True:
    timer = cv2.getTickCount()
    # Capture the video frame by frame
    ret, frame = vid.read()

    # The black region in the mask has the value of 0,
    # so when multiplied with original image removes all non-blue regions
    result = cv2.bitwise_and(frame, frame, mask=Functions.maskGen(np,frame))

    #----------------------------------------- Tracker

    #Functions.drawColliderBox(result,collideBox)
    #Update tracker
    #if not trackinit:
    trackok = tracker.init(result, trackBox)
    #trackinit = True

    trackok, trackBox = tracker.update(result)

    if trackok:
        Functions.drawTrackBox(result, trackBox)
        #print(Functions.boxCordsToString(trackBox))
        sock.sendto((Functions.boxCordsToString(trackBox)).encode(), (UDP_IP, UDP_PORT))

    else:
        # Tracking failure
        cv2.putText(result, "Tracking failure detected", (100, 380), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)

    #if Functions.detectRectangleCollision(collideBox,trackBox):
    #   cv2.putText(result, "Collision", (100, 200), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)


    # Display result
    #cv2.imshow("Tracking", result)

    # Calculate and display fps
    #fps = cv2.getTickFrequency()/(cv2.getTickCount()- timer)
    #cv2.putText(result,str(fps),(75,50),cv2.FONT_HERSHEY_COMPLEX,0.7,(255,255,255),2)

    #Display options

    cv2.imshow('frame', frame)
    #cv2.imshow('mask', mask)
    cv2.imshow('result', result)

    #break video capture
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break



# After the loop release the cap object
vid.release()
cv2.destroyAllWindows()

# p1 = (int(bbox[0]), int(bbox[1]))
# p2 = (int(bbox[0] + bbox[2]), int(bbox[1] + bbox[3]))
# cv2.rectangle(result, p1, p2, (255, 0, 0), 2, 1)