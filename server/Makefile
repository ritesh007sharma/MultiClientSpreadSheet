
BUILD_DIR = build # Directory to place the build output

EXEC = build/Server

C_FLAGS = -pthread -std=c++11 -g

INCLUDE = build/SocketState.o build/NetController.o build/dependencyGraph.o \
           build/JSONEncoder.o build/spreadsheet.o

default: mdir SocketState.o NetController.o dependencyGraph.o spreadsheet.o JSONEncoder.o
	g++ -o $(EXEC) Server.cpp $(INCLUDE) $(C_FLAGS)

mdir:
	mkdir -p $(BUILD_DIR)

SocketState.o:
	g++ -c -o build/SocketState.o SocketState.cpp $(C_FLAGS)

NetController.o:
	g++ -c -o build/NetController.o NetController.cpp $(C_FLAGS)

dependencyGraph.o:
	g++ -c -o build/dependencyGraph.o dependencyGraph.cpp $(C_FlAGS)

spreadsheet.o:
	g++ -c -o build/spreadsheet.o spreadsheet.cpp $(C_FlAGS)

JSONEncoder.o:
	g++ -c -o build/JSONEncoder.o JSONEncoder.cpp $(C_FlAGS)

JSONDecoder.o:
	g++ -c -o build/JSONDecoder.o JSONDecoder.cpp $(C_FlAGS)

clean:
	rm -r $(BUILD_DIR)
