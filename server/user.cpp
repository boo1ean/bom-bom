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
}
