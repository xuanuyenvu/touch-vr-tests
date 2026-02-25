import cv2
import numpy as np

squareX = 3
squareY = 5
height = 1400
width = int(height * (squareX / squareY) * 1.1)

# Chọn dictionary ArUco
dictionary = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_4X4_50)

# Tạo ChArUco board
board = cv2.aruco.CharucoBoard(
    (squareX, squareY),       
    0.04, 
    0.02,          
    dictionary
)

img = board.generateImage(
    (width, height),
    marginSize=100)

cv2.imwrite("charuco_board.png", img)

cv2.imshow("ChArUco Board", img)
cv2.waitKey(0)
cv2.destroyAllWindows()
