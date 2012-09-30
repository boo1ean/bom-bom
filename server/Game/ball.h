#ifndef BALL_H
#define BALL_H

#include "sceneitem.h"

class Scene;

class Ball : public SceneItem
{
    Q_OBJECT
public:
    Ball(Scene* scene, float r, QObject *parent = 0);
    
signals:
    
private slots:
    
};

#endif // BALL_H
