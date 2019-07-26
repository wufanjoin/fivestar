


#import <BMKLocationKit/BMKLocationComponent.h>

@interface BaiduLocationSdkMgr: NSObject<BMKLocationAuthDelegate,BMKLocationManagerDelegate>
{
}
@property(nonatomic, strong) BMKLocationManager *_locationManager;
@property(nonatomic, copy) BMKLocatingCompletionBlock completionBlock;
+(id)Ins;
@end

