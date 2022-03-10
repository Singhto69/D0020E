import numpy
import packages.math.mathFunctions as mathf
import packages.math.Matrix as matrix

m = matrix.Matrix()

while True:
    m.setDimension((2, 4))
    # x = numpy.array([1, 2, 3, 4])
    x = numpy.random.randint(10, size=(1, 4))
    m.receive(x)
