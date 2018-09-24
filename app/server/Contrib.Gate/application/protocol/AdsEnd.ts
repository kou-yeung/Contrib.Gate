function AdsEnd(params, context, done)
{
    // 受信データをパースする
    let s = AdsEndSend.Parse(params);
    //s.id
    GetUser(context, (user) =>
    {
        let admin = GetAdmin(context);
        new Entities.Ads(admin, user).refresh(ads =>
        {
            // 返信
            if (ads.vaild(s.id))
            {
                ads.clear(() => {

                    if (ads.reward == AdReward.Unknown) {
                        done(ApiError.Create(ErrorCode.Common, "不明な報酬").Pack());
                        return;
                    } else if (ads.reward == AdReward.Hatch) {
                        new Entities.Hatch(user, ads.param).refresh(hatch =>
                        {
                            let item = hatch.item;
                            item.timeRequired -= 30 * 60;
                            hatch.item = item;
                            hatch.bucket.save(() => {
                                let r = new AdsEndReceive();
                                r.item = item;
                                done(r.Pack());
                                return;
                            });
                        });
                        return;
                    } else {
                        return;
                    }
                });
            } else
            {
                ads.clear(() => {
                    let r = ApiError.Create(ErrorCode.InvalidAdsCode);
                    done(r.Pack());
                });
            }
        });
    });
}