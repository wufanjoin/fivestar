//
//  BaiduLocationSdkMgr.m
//  Unity-iPhone
//
//  Created by 吴凡 on 2019/6/27.
//

#include "BaiduLocationSdkMgr.h"
#include "SDKAppController.h"

@implementation BaiduLocationSdkMgr

static BaiduLocationSdkMgr *ins;
NSString* _ak=@"KgMq6afDFXxLt0YSRoA4sIeA3uzMeeSq";
NSString* _callLocationfunctionName  = @"LocationCall";

+(id)Ins
{
    
    if (!ins) {
        ins = [[BaiduLocationSdkMgr alloc] init];
        [[BMKLocationAuth sharedInstance] checkPermisionWithKey:_ak authDelegate:self];
        [ins initLocation];
    }
    return ins;
}

-(void)initLocation
{
    self._locationManager = [[BMKLocationManager alloc] init];
    
    self._locationManager.delegate = self;
    
    self._locationManager.coordinateType = BMKLocationCoordinateTypeBMK09LL;
    self._locationManager.distanceFilter = kCLDistanceFilterNone;
    self._locationManager.desiredAccuracy = kCLLocationAccuracyBest;
    self._locationManager.activityType = CLActivityTypeAutomotiveNavigation;
    self._locationManager.pausesLocationUpdatesAutomatically = NO;
    self._locationManager.allowsBackgroundLocationUpdates = YES;
    self._locationManager.locationTimeout = 10;
    self._locationManager.reGeocodeTimeout = 10;
    self.completionBlock = ^(BMKLocation *location, BMKLocationNetworkState state, NSError *error)
    {
        if (error)
        {
            NSLog(@"locError:{%ld - %@};", (long)error.code, error.localizedDescription);
            [SDKAppController CallUnity:_callLocationfunctionName param:@""];
        }
        if (location.location) {
            
            NSLog(@"LOC = %@",location.location);
            NSLog(@"LOC ID= %@",location.locationID);
            // float latFloat=location.location.coordinate.latitude;//纬度
            // float logFloat=location.location.coordinate.longitude; //经度
            //  NSString* description=[location.rgcData description];//地址信息
            NSString* locationCallInfo=[NSString stringWithFormat:@"%@%@|%.6f|%.6f",location.rgcData.district,location.rgcData.street,location.location.coordinate.longitude,location.location.coordinate.latitude];
            [SDKAppController CallUnity:_callLocationfunctionName param:locationCallInfo];
            NSLog(@"unity 定位成功%@",locationCallInfo);
        }
        
    };
    
}
//定位回调 不实现应该也没关系
- (void)BMKLocationManager:(BMKLocationManager * _Nonnull)manager doRequestAlwaysAuthorization:(CLLocationManager * _Nonnull)locationManager
{
    [locationManager requestAlwaysAuthorization];
}

-(void)locationBaidu
{
    NSLog(@"unity 请求定位");
    [self._locationManager requestLocationWithReGeocode:YES withNetworkState:NO completionBlock:self.completionBlock];
}
extern "C"
{
    //定位信息
    int IOSGetLocation()
    {
       [[BaiduLocationSdkMgr Ins] locationBaidu];
        return 0;
    }
}
@end
