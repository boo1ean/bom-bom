#ifndef BALL_H
#define BALL_H

#include <QObject>
#include <Box2D.h>

class Scene;

class Ball : public QObject
{
    Q_OBJECT
public:
    Ball(Scene* scene, float r, QObject *parent = 0);

    void update();

private:

    b2Body *_body;
    
signals:
    
private slots:
    
};

#endif // BALL_H
