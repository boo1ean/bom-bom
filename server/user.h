#ifndef USER_H
#define USER_H

#include <QObject>

QT_BEGIN_NAMESPACE
class QString;
class QTcpSocket;
QT_END_NAMESPACE

enum USER_TYPE {
    USER_BADA,
    USER_ANDROID,
    USER_WP7,
    USER_IPHONE,
    USER_SYMBIAN,
    USER_OBSERVER
};

enum CMD {
    CMD_INIT,
    CMD_NAME,
    CMD_ACC_DATA,
    CMD_NOTIFY,
    CMD_SCENE_STATE
};

class User : public QObject
{
    Q_OBJECT
public:
    explicit User(QTcpSocket * connection, QObject *parent = 0);
    QString getName();
    void setName(QString);

    char getType();
    
signals:

public slots:
    void onReadyRead();

private:

    void setType(char);

    void cmd_init();
    void cmd_name();
    void cmd_accData();
    void cmd_sceneState();
    void cmd_notify();



    char       type;
    QString    name;
    QByteArray data;

    QTcpSocket * connection;
};

#endif // USER_H
