//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class EggListSend {


	// params -> EggListSend
	static Parse(params : any): EggListSend {
		return JSON.parse(params.data[0]) as EggListSend;
	}
}

class EggListReceive {
    items: EggItem[]; // 所持タマゴ一覧
	
	// EggListReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function EggList(params, context, done) {

    // 受信データをパースする
    let s = EggListSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new EggListReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/