//
//  WeChatSdkMgr.m
//  Unity-iPhone
//
//  Created by 吴凡 on 2019/6/27.
//

#include "WeChatSdkMgr.h"
#include "SDKAppController.h"
#include<CommonCrypto/CommonDigest.h>

@implementation WeChatSdkMgr

static WeChatSdkMgr *ins;
NSString* _wxAppId=@"wxbce11d00de2d0ebd";
NSString* _wxSecreet=@"53ff147c27083dc7d62185668c60c1cf";
NSString* _loginCallFuncionName = @"WxLoginCall";

+(id)Ins
{
    
    if (!ins) {
        ins = [[WeChatSdkMgr alloc] init];
        [WXApi registerApp:_wxAppId ];//注册appid
    }
    return ins;
}

-(void) onReq:(BaseReq*)req
{
    //这个方法不会被调用 因为原来的方法 被弃用了
}

- (void)onResp:(BaseResp*)resp
{
    NSLog(@"unity 调用 onResp 收到回调");
    if([resp isKindOfClass:[SendAuthResp class]]) // 登录授权
    {
        SendAuthResp *temp = (SendAuthResp*)resp;
        if(temp.code!=nil)
        {
            [self getAccessToken:temp.code];
        }
    }
}



//获取玩家信息 回调给unity 现有分享没有办法获取 成功与否
- (void)getAccessToken:(NSString *)code
{
    NSString *path = [NSString stringWithFormat:@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=%@&secret=%@&code=%@&grant_type=authorization_code",_wxAppId,_wxSecreet,code];
    NSURLRequest *request = [[NSURLRequest alloc] initWithURL:[NSURL URLWithString:path] cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0];
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    [NSURLConnection sendAsynchronousRequest:request queue:queue completionHandler:
     ^(NSURLResponse *response,NSData *data,NSError *connectionError)
     {
         if(connectionError != NULL)
         {
         }
         else
         {
             if (data != NULL)
             {
                 NSError *jsonParseError;
                 
                 NSDictionary *responseData = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&jsonParseError];
                 NSString *accessToken = [responseData valueForKey:@"access_token"];
                 NSString *openid = [responseData valueForKey:@"openid"];
                 NSString *Str = [NSString stringWithFormat:@"%@|%@",accessToken,openid];
                 [SDKAppController CallUnity:_loginCallFuncionName param:Str];
             }
         }
     }];
}

+ (NSData *)imageWithImage:(UIImage*)image scaledToSize:(CGSize)newSize;
{
    UIGraphicsBeginImageContext(newSize);
    [image drawInRect:CGRectMake(0,0,newSize.width,newSize.height)];
    UIImage* newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return UIImageJPEGRepresentation(newImage, 0.8);
}

- (void)wxLogin
{
    SendAuthReq* req = [[SendAuthReq alloc] init] ;
    req.scope = @"snsapi_userinfo";
    req.state = @"only123";
    [WXApi sendAuthReq:req viewController:[[SDKAppController Ins] getViewController] delegate:self];
}

//微信支付签名函数
- (NSString *)partnerSignOrder:(NSDictionary*)paramDic{
    NSArray *keyArray = [paramDic allKeys];
    NSMutableArray *sortedKeyArray = [NSMutableArray arrayWithArray:keyArray];
    [sortedKeyArray sortUsingComparator:^NSComparisonResult(NSString* key1, NSString* key2) {
        return [key1 compare:key2];
    }];
    NSMutableString *paramString = [NSMutableString stringWithString:@""];
    // 拼接成 A=B&X=Y
    for (NSString *key in sortedKeyArray){
        if ([paramDic[key] length] != 0){
            [paramString appendFormat:@"&%@=%@", key, paramDic[key]];
        }
    }
    if ([paramString length] > 1){
        [paramString deleteCharactersInRange:NSMakeRange(0, 1)];    // remove first '&'
    }
    [paramString appendFormat:@"&key=%@", @"123456789123456789123456789abcde"];//app密钥
    NSLog(@"签名字符串:%@",paramString);
    return [[self signString:paramString] uppercaseString];
}

//字符串签名md5加密函数
- (NSString *)signString:(NSString*)origString{
    const char *original_str = [origString UTF8String];
    unsigned char result[32];
    CC_MD5(original_str, (CC_LONG)strlen(original_str), result);//调用md5
    NSMutableString *hash = [NSMutableString string];
    for (int i = 0; i < 16; i++){
        [hash appendFormat:@"%02X", result[i]];
    }
    return hash;
}





extern "C"
{
    int shareType_Friend=1;
    int shareType_Circle=2;
    
    //微信登陆
    void IOSWeChatLogin()
    {
        [WXApi registerApp:_wxAppId ];//注册appid
        [[WeChatSdkMgr Ins] wxLogin] ;//调用为微信登陆
    }
    //分享图片
    void IOSWeChatShareImage(char* imagePath, char* title, char* desc, int shadeType)
    {
        [WXApi registerApp:_wxAppId ];//注册appid
        NSLog(@"unity 分享图片");
        WXMediaMessage *message=[WXMediaMessage message];
        NSString *Str=[NSString stringWithUTF8String:imagePath];
        UIImage *imag =  [UIImage imageWithContentsOfFile:Str];;
        [message setThumbImage:[UIImage imageWithData:[WeChatSdkMgr imageWithImage:imag scaledToSize:CGSizeMake(300, 300)]]];
        WXImageObject *imageObject=[WXImageObject object];
        //imageObject.imageData=[UnityAppController imageWithImage:imag scaledToSize:CGSizeMake(300, 300)];
        imageObject.imageData=UIImagePNGRepresentation(imag);
        message.mediaObject=imageObject;
        
        SendMessageToWXReq* req=[[SendMessageToWXReq alloc]init];
        req.bText=NO;
        req.message=message;
        if(shadeType==shareType_Friend)
        {
            req.scene = WXSceneSession;//好友
        }
        else
        {
            req.scene = WXSceneTimeline;//朋友圈
        }
        [WXApi sendReq:req];
    }
    
    //分享链接
    void IOSWeChatShareUrl(char* url, char* title, char* description, int shareType)
    {
        [WXApi registerApp:_wxAppId ];//注册appid
        NSLog(@"unity 分享链接");
        WXMediaMessage *message=[WXMediaMessage message];
        message.title=[NSString stringWithUTF8String:title];
        message.description=[NSString stringWithUTF8String:description];
        
        
        //获取图片网络地址
        //        NSURL *imageUrl = [NSURL URLWithString: @"图片网络地址"];// 获取的图片地址
        //        UIImage *image = [UIImage imageWithData: [NSData dataWithContentsOfURL:imageUrl]];
        
        //获取icon
        NSDictionary *infoPlist = [[NSBundle mainBundle] infoDictionary];
        NSString *icon = [[infoPlist valueForKeyPath:@"CFBundleIcons.CFBundlePrimaryIcon.CFBundleIconFiles"] lastObject];
        UIImage* image = [UIImage imageNamed:icon];
        
        
        [message setThumbImage:image];
        WXWebpageObject *webpageObject=[WXWebpageObject object];
        webpageObject.webpageUrl=[NSString stringWithUTF8String:url];
        
        message.mediaObject=webpageObject;
        
        SendMessageToWXReq* req=[[SendMessageToWXReq alloc]init];
        req.bText=NO;
        req.message=message;
        if(shareType==shareType_Friend)
        {
            req.scene = WXSceneSession;//好友
        }
        else
        {
            req.scene = WXSceneTimeline;//朋友圈
        }
        [WXApi sendReq:req];
    }
    
    
    NSString* _wxPayAppId=@"wxd22448210abef405";
     NSString*  PartnerKey =@"123456789123456789123456789abcde";//商户秘钥
    //微信发起支付
    
    void IOSWeChatPay(char* prieid, char* nonstr)
    {
        NSLog(@"unity 发起支付");
        [WXApi registerApp:_wxPayAppId ];//注册appid
        //调起微信支付
        PayReq* req             = [[PayReq alloc] init];
        req.openID=_wxPayAppId;
        req.partnerId           = @"1542580991";
        req.prepayId            = [NSString stringWithUTF8String:prieid];
        req.nonceStr            = [NSString stringWithUTF8String:nonstr];
        req.package             = @"Sign=WXPay";
        //获取时间戳
        NSDate *datenow = [NSDate date];//现在时间,你可以输出来看下是什么格式
        NSString *timeSp = [NSString stringWithFormat:@"%d", (long)[datenow timeIntervalSince1970]];
        UInt32 num;
        sscanf([timeSp UTF8String], "%u", &num);
        req.timeStamp           = num;
        //NSLog(@"timeStamp:%@",timeSp);
        
       // NSString* signStr=[NSString stringWithFormat:@"appid=%@&nonceStr=%@&package=%@&partnerid=%@&prepayid=%@&timestamp=%d&key=%@",_wxPayAppId,req.nonceStr,req.package,req.partnerId, req.prepayId,req.timeStamp,PartnerKey];;
        //   NSLog(@"signStr:%@",signStr); 不能自己直接拼接
   
         NSMutableDictionary* payDic = [NSMutableDictionary dictionary];
         [payDic setValue:_wxPayAppId forKey:@"appid"];
         [payDic setValue:req.nonceStr  forKey:@"noncestr"];
         [payDic setValue:req.package  forKey:@"package"];
         [payDic setValue:req.partnerId forKey:@"partnerid"];
         [payDic setValue:req.prepayId forKey:@"prepayid"];
         [payDic setValue:timeSp forKey:@"timestamp"];
         req.sign = [[[WeChatSdkMgr Ins] partnerSignOrder:payDic] uppercaseString];
         [WXApi sendReq:req];
    }
}
@end


