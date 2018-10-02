function AdsEnd(params, context, done)
{
    // 受信データをパースする
    let s = AdsEndSend.Parse(params);

    // 空き文字列で送信してきた
    if (s.id.length <= 0) {
        done(ApiError.Create(ErrorCode.Common, "不明な報酬").Pack());
        return;
    }
    //s.id
    GetUser(context, (user) =>
    {
        let admin = GetAdmin(context);
        new Entities.Ads(admin, user).refresh(ads =>
        {
            // 有効性チェック
            if (!ads.vaild(s.id)) {
                done(ApiError.Create(ErrorCode.InvalidAdsCode).Pack());
                return;
            }

            // 返信
            ads.clear(() => {
                switch (ads.reward) {
                    case AdReward.Hatch:
                        HatchReward(user, ads, hatch => {
                            let r = new AdsEndReceive();
                            r.hatch = [hatch];
                            done(r.Pack());
                        });
                        break;
                    case AdReward.Unit:
                        UnitReward(user, ads, unit => {
                            let r = new AdsEndReceive();
                            r.unit = [unit];
                            done(r.Pack());
                        });
                        break;
                    default:
                        UnknownReward(user, ads, () => {
                            done(new AdsEndReceive().Pack());
                        });
                        break;
                }
            });
        });
    });
}

function UnknownReward(user: KiiUser, ads: Entities.Ads, done: () => void) {
    done();
}
// 孵化
function HatchReward(user: KiiUser, ads: Entities.Ads, done: (hatch: HatchItem) => void) {
    new Entities.Hatch(user, ads.param).refresh(hatch => {
        let item = hatch.item;
        item.startTime -= 30 * 60;  // 開始時間を過去に!!(計算上はこっちのほうがしやすいです)
        hatch.item = item;
        hatch.bucket.save(() => {
            done(item);
        });
    });
}

// ユニット
function UnitReward(user: KiiUser, ads: Entities.Ads, done: (unit: UnitItem) => void) {
    new Entities.Units(user).refresh(units => {
        let index = parseInt(ads.param);
        let items = units.items;
        let expiration = ((3600 * 24) * 30);
        items[index].expirationDate = Util.Time.ServerTime.current + expiration;
        units.items = items;    // 更新
        units.bucket.save(() => {
            done(items[index]);
        });
    });
}
