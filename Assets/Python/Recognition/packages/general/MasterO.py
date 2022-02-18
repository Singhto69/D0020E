class MasterO:
    def __init__(self):
        self.slaves = []

    def stringParser(self, input):
        return input.split()

    def alter(self, params, *objects):
        params = self.stringParser(params)

