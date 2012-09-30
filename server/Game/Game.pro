#-------------------------------------------------
#
# Project created by QtCreator 2012-09-21T00:18:55
#
#-------------------------------------------------

QT       += core gui network opengl

TARGET = Game
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
    observer.cpp \
    scene.cpp \
    wall.cpp \
    ball.cpp \
    plank.cpp \
    sceneitem.cpp

HEADERS  += mainwindow.h \
    user.h \
    server.h \
    connection.h \
    game.h \
    screenobject.h \
    clientscreenobject.h \
    player.h \
    client.h \
    observer.h \
    command.h \
    clienttype.h \
    scene.h \
    wall.h \
    ball.h \
    plank.h \
    sceneitem.h

RESOURCES += \
    tiles.qrc

INCLUDEPATH += . \
    .. \
    ../Box2D

LIBS += -lBox2D

debug {
    LIBS += -L../Box2D/debug
}

release {
    LIBS += -L../Box2D/release
}
