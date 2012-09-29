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

class User : public QObject
{
    Q_OBJECT
public:
    explicit User(QTcpSocket * connection, QObject *parent = 0);
    QString getName();
    void setName(QString);
    
signals:

public slots:
    void onReadyRead();

private:
    QString name;
    
};

#endif // USER_H
