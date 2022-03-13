import numpy


class Matrix:
    def __init__(self):
        self.matrix = numpy.array([])
        self.columns = None
        self.rows = None
        self.shape = None
        self.outputval = numpy.array([0, 0, 0, 0])

    def getoutputval(self):
        return self.outputval

    def receive(self, array):
        if self.columns is not None and self.rows is not None:
            array[0] = abs(array[0] - 500) # Hardcoded inversion
            self.reshape(array.astype(int))

    def reshape(self, array):
        if self.columns is not None and self.rows is not None:
            # self.shape = self.matrix.shape
            self.matrix = self.matrix.flatten()

            length = len(self.matrix)
            condA = length >= self.rows * self.columns
            condB = length < self.rows * self.columns
            condC = length == 0

            if not condA:
                self.matrix = numpy.append(self.matrix, array).astype(int)
                if condB and not condC:
                    self.matrix = numpy.reshape(self.matrix, (int(len(self.matrix) / self.columns), self.columns))
                return

            self.matrix = numpy.append(self.matrix, array, axis=0)
            self.matrix = numpy.reshape(self.matrix, (int(len(self.matrix) / self.columns), self.columns))
            self.matrix = numpy.delete(self.matrix, 0, 0)
            interexpolate = self.interExtrapolate()
            if interexpolate is not None:
                self.outputval = interexpolate
            # print(self.outputval)

    def interExtrapolate(self):
        shape = self.matrix.shape
        if shape[0] == self.rows and shape[1] == self.columns:
            x1, y1, x2, y2 = self.matrix[-2][0], self.matrix[-2][1], self.matrix[-1][0], self.matrix[-1][1]
            # (x1 + x2 ) /2 , ( y1 + y2) / 2 <-- Midpoint
            # y = ( deltay / deltax ) * x + b
            # y2 = m*x1 + b
            if x2 - x1 != 0:
                m = int(y2 - y1) / int(x2 - x1)
                mx2 = int(m * x2)
                b = int(y2 + ((-1) * mx2))

                # Interpolate x1.5 and y1.5
                x1dot5 = int((x1 + x2) / 2)
                y1dot5 = int(m * x1dot5 + b)

                # extrapolate x3 and y3
                x3 = int(x2 + ((x2 - x1) * 0.5))
                y3 = int(m * x3 + b)
                return numpy.array([x1dot5, y1dot5, 80, 100]).astype(int)
        else:
            return numpy.array([0, 0, 0, 0])

    def setDimension(self, tuple):
        self.rows, self.columns = tuple[0], tuple[1]

    def getOutputVal(self):
        return self.outputval
