//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class HatchReserveSend {
    uniqid: string; // タマゴ Uniqid

	// params -> HatchReserveSend
	static Parse(params : any): HatchReserveSend {
		return JSON.parse(params.data[0]) as HatchReserveSend;
	}
}

class HatchReserveReceive {
    item: HatchItem; // 孵化情報
	
	// HatchReserveReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function HatchReserve(params, context, done) {

    // 受信データをパースする
    let s = HatchReserveSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new HatchReserveReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/