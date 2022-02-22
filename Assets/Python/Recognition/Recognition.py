import cv2
import numpy
import packages.general.Functions as Functions
import packages.network.SocketO as SocketO
import packages.opencv.Drawtools as Drawtools
import packages.opencv.Masks as Masks
import packages.opencv.FrameObjectSingle as FrameO
import packages.opencv.TrackerO as TrackerO

sock = SocketO.SocketO()
sock.setIP("192.168.10.166")
sock.setPort(5065)
sock.setUDP()
sock.createSocket()

# Define a video capture frame object
cam = FrameO.FrameObjectSingle("cam")
cam.ops("set input cam")

# Define a inverted frame object
inverted = FrameO.FrameObjectSingle("inv")

# Define a masked frame object
masked = FrameO.FrameObjectSingle("masked")

# For tracker... pip install opencv-contrib-python==3.4.11.45
trackO = TrackerO.TrackerO()
# trackO.ops("set tracker csrt")
trackO.ops("set tracker trackbox", numpy.array([200, 150, 120, 120]))
trackO.ops("set tracker rootbox", numpy.array([200, 150, 120, 120]))

while True:
    # timer = cv2.getTickCount()
    cam.ops("set frame input")
    camFrame = cam.ops("return frame")
    inverted.ops("set input frame", camFrame)
    masked.ops("set input frame", camFrame)
    inverted.ops("set frame inv")
    masked.ops("set frame mergedmaskbitand", Masks.maskRedTuned1(), Masks.maskRedTuned2())
    trackO.ops("init tracker", masked.ops("return frame"))
    trackO.ops("update tracker", masked.ops("return frame"))
    # sock.socket.sendto((Functions.boxCordsToString(trackBox,"Seb")).encode(), (sock.ip, sock.port))

    # Calculate and display fps
    # fps = cv2.getTickFrequency()/(cv2.getTickCount()- timer)
    # cv2.putText(result,str(fps),(75,50),cv2.FONT_HERSHEY_COMPLEX,0.7,(255,255,255),2)

    cam.ops("show frame")
    inverted.ops("show frame")
    masked.ops("show frame")

    # break video capture
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# After the loop release the cap object
# vid.release()
# cv2.destroyAllWindows()
