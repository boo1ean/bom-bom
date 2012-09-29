//
//  AirHockeyAppDelegate.h
//  AirHockey
//
//  Created by binary studio on 9/29/12.
//  Copyright 2012 binary-studio. All rights reserved.
//

#import <UIKit/UIKit.h>

@class AirHockeyViewController;

@interface AirHockeyAppDelegate : NSObject <UIApplicationDelegate> {
    UIWindow *window;
    AirHockeyViewController *viewController;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet AirHockeyViewController *viewController;

@end

