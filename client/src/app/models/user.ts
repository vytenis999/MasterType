import { Basket } from "./basket";
import { Loved } from "./loved";

export interface User {
    email: string;
    token: string;
    basket?: Basket;
    loved?: Loved;
    roles?: string[];
}