//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class UnitUpdateSend {
    items: UnitItem[]; // Unit情報一覧

	// params -> UnitUpdateSend
	static Parse(params : any): UnitUpdateSend {
		return JSON.parse(params.data[0]) as UnitUpdateSend;
	}
}

class UnitUpdateReceive {

	
	// UnitUpdateReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function UnitUpdate(params, context, done) {

    // 受信データをパースする
    let s = UnitUpdateSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new UnitUpdateReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/