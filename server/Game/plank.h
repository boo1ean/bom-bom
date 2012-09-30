#ifndef PLANK_H
#define PLANK_H

#include "sceneitem.h"

class Scene;

class Plank : public SceneItem
{
    Q_OBJECT
public:
    Plank(Scene *scene, const b2Vec2& c, float r, QObject *parent = 0);
    
signals:
    
public slots:
    
};

#endif // PLANK_H
