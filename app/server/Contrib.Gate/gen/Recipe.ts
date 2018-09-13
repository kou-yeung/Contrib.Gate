//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class RecipeSend {
    identify: number; // 実行したいレシピID

	// params -> RecipeSend
	static Parse(params : any): RecipeSend {
		return JSON.parse(params.data[0]) as RecipeSend;
	}
}

class RecipeReceive {
    identify: number; // 獲得されたもの
	
	// RecipeReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Recipe(params, context, done) {

    // 受信データをパースする
    let s = RecipeSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new RecipeReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/