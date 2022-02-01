import cv2


def boxCordsToString(array):
    #newstr = ""
    newstr = str((int(abs(array[0]-500)))) + ":" + str((int(abs((array[1]/2)-250)))) + ":" +\
             str((int(array[2]))) + ":" + str((int(array[3]))) + ":"

    #for i in array:
        #if count == 0:
            #i = abs(i-500)
            #count = count + 1
        #if count == 1:
            #i = abs(i-250)
            #count = count + 1
        #newstr = newstr + str((int(i))) + ":"
    #print(newstr)
    return newstr




def detectRectangleCollision( box1 , box2):
    #collidebox
    b1x ,b1y ,b1w ,b1h = int(box1[0]), int(box1[1]), int(box1[2]), int(box1[3])
    #trackbox
    b2x ,b2y , b2w ,b2h = int(box2[0]), int(box2[1]), int(box2[2]), int(box2[3])

    #bottom left x , y , top right x , y
    #B1 = [ b1x, b1y + b1h , b1x + b1w , b1y]
    #B2 = [b2x , b2y+b2h , b2x +b2w , b2y]

    b1topleft = [b1x,b1y]
    b1botright = [b1x+b1w ,b1y+b1h]
    b2topleft = [b2x,b2y]
    b2botright = [b2x+b2w,b2y+b2h]

    if (b1topleft[0] == b1botright[0] or b1topleft[1] == b1topleft[1] or b2topleft[0] == b2botright[0] or b2topleft[1] == b2botright[1]):
        # the line cannot have positive overlap
        print("Hi1")
        return False

    # If one rectangle is on left side of other
    if (b1topleft[0] >= b2botright[0] or b2topleft[0] >= b1botright[0]):
        print("Hi2")
        return False

    # If one rectangle is above other
    if (b1botright[1] >= b2topleft[1] or b2botright[1] >= b1topleft[1]):
        print("Hi3")
        return False

    return True




    #if (B1[0] >= B2[2]) or (B1[2] <= B2[0]) or (B1[3] <= B2[1]) or (B1[1] >= B2[3]):
        #return True
    #else:
      #  return False

def drawColliderBox(frame, box):
    x, y, w, h = int(box[0]), int(box[1]), int(box[2]), int(box[3])
    cv2.rectangle(frame, (x, y), ((x + w), (y + h)), (0, 0, 255), 3, 1)
    cv2.putText(frame, "colliderBox", (500, 380), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)
    cv2.putText(frame, "X: " + str(x), (500, 400), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)
    cv2.putText(frame, "Xw: " + str(x+w), (500, 420), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)
    cv2.putText(frame, "Y: " + str(y), (500, 440), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)
    cv2.putText(frame, "Yh: " + str(y+h), (500, 460), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)

def drawTrackBox(frame, box):
    x ,y ,w ,h = int(box[0]) , int(box[1]) , int(box[2]) , int(box[3])
    cv2.rectangle(frame, (x, y), ((x + w) , (y + h)), (0, 255, 0), 3, 1)
    cv2.putText(frame, "trackBox", (100, 380), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 255, 0), 2)
    cv2.putText(frame, "X: " + str(x), (100, 400), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 255, 0), 2)
    cv2.putText(frame, "Xw: " + str(x+w), (100, 420), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 255, 0), 2)
    cv2.putText(frame, "Y: " + str(y), (100, 440), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 255, 0), 2)
    cv2.putText(frame, "Yh: " + str(y+h), (100, 460), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 255, 0), 2)

def maskGen(numpy, frame):
    # It converts the BGR color space of image to HSV color space
    hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
    #kolla upp hue saturation RGB
    # lower mask(0 - 10)
    lower_red = numpy.array([0, 50, 50])
    upper_red = numpy.array([10, 255 , 255])
    mask0 = cv2.inRange(hsv, lower_red, upper_red)

    # upper mask(170 - 180)
    lower_red = numpy.array([170, 50, 50])
    upper_red = numpy.array([180, 255, 255])
    mask1 = cv2.inRange(hsv, lower_red, upper_red)

    mask = mask0 + mask1
    # preparing the mask to overlay
    mask = cv2.inRange(hsv, lower_red, upper_red)
    return mask