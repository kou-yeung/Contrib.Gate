//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class PowerupSend {
    uniqid: string; // ペットUniqid
    items: InventoryItem[]; // 使用予定アイテム一覧

	// params -> PowerupSend
	static Parse(params : any): PowerupSend {
		return JSON.parse(params.data[0]) as PowerupSend;
	}
}

class PowerupReceive {
    items: InventoryItem[]; // 使用したアイテム一覧
    pet: PetItem; // 更新したペット情報
	
	// PowerupReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Powerup(params, context, done) {

    // 受信データをパースする
    let s = PowerupSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new PowerupReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/