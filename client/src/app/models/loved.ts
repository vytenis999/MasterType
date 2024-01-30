export interface Loved {
    id: number
    buyerId: string
    items: LovedItem[]
  }
  
  export interface LovedItem {
    productId: number
    name: string
    price: number
    pictureUrl: string
  }