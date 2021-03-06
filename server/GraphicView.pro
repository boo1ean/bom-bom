#-------------------------------------------------
#
# Project created by QtCreator 2012-09-21T00:18:55
#
#-------------------------------------------------

QT       += core gui network opengl

TARGET = GraphicView
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    user.cpp \
    server.cpp \
    connection.cpp \
    game.cpp \
    screenobject.cpp \
    clientscreenobject.cpp \
    player.cpp \
    client.cpp \
    observer.cpp

HEADERS  += mainwindow.h \
    user.h \
    server.h \
    connection.h \
    game.h \
    screenobject.h \
    clientscreenobject.h \
    player.h \
    client.h \
    ClientType.h \
    observer.h \
    command.h

RESOURCES += \
    tiles.qrc
