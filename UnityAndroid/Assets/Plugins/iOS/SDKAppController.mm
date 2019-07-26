
#include "SDKAppController.h"
//#include "WXApi.h"
#include "WeChatSdkMgr.h"
#include "BaiduLocationSdkMgr.h"

IMPL_APP_CONTROLLER_SUBCLASS (SDKAppController)

@implementation SDKAppController

static SDKAppController *interfaceIns;


+(id)Ins
{
    if (!interfaceIns) {
        interfaceIns = [[SDKAppController alloc] init];
    }
    return interfaceIns;
}



NSString *  _urlOpenAppCallName  = @"UrlOpenAppCall";
//外部打开这个app会走着
- (BOOL)application:(UIApplication*)app openURL:(NSURL*)url options:(NSDictionary<NSString*, id>*)options
{
    NSString * urlStr = [url absoluteString];
     if ([urlStr rangeOfString:@"roomId"].location != NSNotFound) {
        [SDKAppController CallUnity:_urlOpenAppCallName param:urlStr];
    }
    return [WXApi handleOpenURL:url delegate:[WeChatSdkMgr Ins]];
}

//要初始化的SDK可以在着调 程序初始化完成后 会走这
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    [WeChatSdkMgr Ins];
    [BaiduLocationSdkMgr Ins];
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}

//获取UIViewController组件
-(UIViewController*)  getViewController
{
    return _rootController;
}


extern "C"
{
    //获取电量 1-100
    int IOSGetBatteryElectric()
    {
        [UIDevice currentDevice].batteryMonitoringEnabled = YES;
        return  [UIDevice currentDevice].batteryLevel*100;
    }
    
    //复制文字到剪贴板
    int IOSCopyClipBoard(char* content)
    {
 
        

        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        pasteboard.string =[NSString stringWithUTF8String:content];
        return 0;
    }
    
    //安装IPA包
    void InstallationIpa(char* url)
    {
        NSString *str = [NSString stringWithUTF8String:url];
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:str]];
    }
}

//ios 调用 unity
+(void)CallUnity:(NSString *)funName param:(NSString *)str
{
   UnitySendMessage("SdkCall", [funName UTF8String], [str UTF8String]);
}
@end

