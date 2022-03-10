from socket import *
import packages.general.Functions as Functions


class SocketO:
    def __init__(self, ip, port):
        self.ip = ip
        self.port = port
        self.socket = None
        self.AF = AF_INET
        self.protocol = None
        self.transmitObj = None

    def setIP(self, ip):
        self.ip = ip

    def setPort(self, port):
        self.port = port

    def createSocket(self, ):
        self.socket = socket(self.AF, self.protocol)

    def setTCP(self):
        self.protocol = SOCK_STREAM

    def setUDP(self):
        self.protocol = SOCK_DGRAM

    def getSocket(self):
        return self.socket

    def setTransmitObj(self, obj):
        self.transmitObj = obj

    def transmithardcode(self):
        x = self.transmitObj.getOutputVal()
        # self.socket.sendto(array.encode('utf8'), (self.ip, self.port))

    def transmit(self, param):
        x = self.transmitObj.getOutputVal()
        if param == "tracker" and type(x) == str:
            self.socket.sendto(x.encode('utf8'), (self.ip, self.port))
        elif param == "matrix":
            x = Functions.boxCordsToString(x, "None")
            self.socket.sendto(x.encode('utf8'), (self.ip, self.port))
