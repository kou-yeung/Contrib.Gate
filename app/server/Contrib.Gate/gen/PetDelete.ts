//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class PetDeleteSend {
    uniqid: string; // ペットUniqid

	// params -> PetDeleteSend
	static Parse(params : any): PetDeleteSend {
		return JSON.parse(params.data[0]) as PetDeleteSend;
	}
}

class PetDeleteReceive {
    items: InventoryItem[]; // 更新したアイテム一覧
	
	// PetDeleteReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function PetDelete(params, context, done) {

    // 受信データをパースする
    let s = PetDeleteSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new PetDeleteReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/