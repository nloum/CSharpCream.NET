JAVAC = javac -O -g
JAR = jar

all: ../cream.jar

../cream.jar: *.java
	rm -rf ../classes
	mkdir ../classes
	$(JAVAC) -d ../classes *.java
	$(JAR) cf ../cream.jar -C ../classes .

clean:
	rm -rf ../cream.jar ../classes *~
