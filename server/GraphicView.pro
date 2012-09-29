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
    game.cpp \
    server.cpp \
    connection.cpp

HEADERS  += mainwindow.h \
    user.h \
    game.h \
    server.h \
    connection.h \
    Command.h

RESOURCES += \
    tiles.qrc
