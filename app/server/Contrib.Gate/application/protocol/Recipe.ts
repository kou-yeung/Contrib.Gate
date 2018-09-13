function Recipe(params, context, done) {

    // 受信データをパースする
    let s = RecipeSend.Parse(params);

    let admin = GetAdmin(context);

    // レシピを取得
    new Entities.Recipe(admin, new Entities.Identify(s.identify)).refresh(recipe => {
        GetUser(context, (user) =>
        {
            new Entities.Inventory(user).refresh(inventory => {
                for (var i = 0; i < recipe.count; i++) {
                    if (inventory.num(recipe.material[i]) < recipe.num[i]) {
                        // 素材が足りない
                        let r = new ApiError(ErrorCode.MaterialLack);
                        done(r.Pack());
                        return;
                    }
                }

                // 素材を減らして
                for (var i = 0; i < recipe.count; i++) {
                    inventory.add(recipe.material[i], -recipe.num[i]);
                }
                // ものを付与する
                switch (recipe.result.Type) {
                    case IDType.Material:
                        inventory.add(recipe.result, 1);
                        break;
                }
                // DBに反映して返信する
                inventory.bucket.save(() => {
                    // 返信
                    let r = new RecipeReceive();
                    r.identify = recipe.result.idWithType;
                    done(r.Pack());
                });
            });
        });
    });

}