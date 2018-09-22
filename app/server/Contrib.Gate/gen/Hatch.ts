//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class HatchSend {
    uniqid: string; // タマゴ Uniqid

	// params -> HatchSend
	static Parse(params : any): HatchSend {
		return JSON.parse(params.data[0]) as HatchSend;
	}
}

class HatchReceive {
    item: PetItem; // 結果
    deleteEgg: EggItem; // 孵化したタマゴ
	
	// HatchReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Hatch(params, context, done) {

    // 受信データをパースする
    let s = HatchSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new HatchReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/