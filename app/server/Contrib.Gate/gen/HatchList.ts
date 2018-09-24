//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class HatchListSend {


	// params -> HatchListSend
	static Parse(params : any): HatchListSend {
		return JSON.parse(params.data[0]) as HatchListSend;
	}
}

class HatchListReceive {
    items: HatchItem[]; // 孵化情報一覧
	
	// HatchListReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function HatchList(params, context, done) {

    // 受信データをパースする
    let s = HatchListSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new HatchListReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/