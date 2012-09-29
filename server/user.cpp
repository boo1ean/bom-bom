#include "user.h"
#include <QDebug>
#include <QtNetwork>

User::User(QTcpSocket * connection, QObject *parent) :
    QObject(parent)
{
    qDebug() << "User.construct";

    connect(connection, SIGNAL(disconnected()),
            connection, SLOT(deleteLater()));

    // Execute moveRect method if there is new data in socket
    connect(connection, SIGNAL(readyRead()),
            this,       SLOT(onReadyRead()));

    this->connection = connection;
}

QString User::getName() {
    qDebug() << "User.getName";
    return name;
}

void User::setName(QString _name) {
    qDebug() << "User.setName";
    name = _name;
}

void User::onReadyRead() {
    qDebug() << "User.onReadyRead";

    data = connection->readAll();

    switch(data.at(0)) {
        case CMD_INIT:
            cmd_init();
        break;

        case CMD_NAME:
            cmd_name();
        break;

        case CMD_ACC_DATA:
            cmd_accData();
        break;

        case CMD_NOTIFY:
            cmd_notify();
        break;

        case CMD_SCENE_STATE:
            cmd_sceneState();
        break;

        default:
        break;
    }
}

void User::cmd_init() {
    qDebug() << "User.cmd_init";
    type = data[1];
}

void User::cmd_name() {
    qDebug() << "User.cmd_name";

}

void User::cmd_accData() {
    qDebug() << "User.cmd_accData";

}

void User::cmd_notify() {
    qDebug() << "User.cmd_notify";

}

void User::cmd_sceneState() {
    qDebug() << "User.cmd_sceneState";

}
