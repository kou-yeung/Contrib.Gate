function AdsEnd(params, context, done)
{
    // 受信データをパースする
    let s = AdsEndSend.Parse(params);
    //s.id
    GetUser(context, (user) =>
    {
        let admin = GetAdmin(context);
        new Entities.Ads(admin, user).refresh(ad1 =>
        {
            // 返信
            if (ad1.vaild(s.id))
            {
                ad1.clear(ad2 => {
                    let r = new AdsEndReceive();
                    r.result = "OK";
                    done(r.Pack());
                });
            } else
            {
                ad1.clear(ad2 => {
                    let r = new AdsEndReceive();
                    r.result = "NG";
                    done(r.Pack());
                });
            }
        });
    });
}