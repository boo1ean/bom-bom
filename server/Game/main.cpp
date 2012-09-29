#include "game.h"
#include <QApplication>
#include <QtOpenGL>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);

    Game game;

    return app.exec();
}
