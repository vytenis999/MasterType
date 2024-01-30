import {
  Card,
  CardActions,
  CardContent,
  CardMedia,
  IconButton,
  Typography,
} from "@mui/material";
import { Product } from "../../app/models/product";
import { Link } from "react-router-dom";
import { LoadingButton } from "@mui/lab";
import { currencyFormat } from "../../app/util/util";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { addBasketItemAsync } from "../basket/basketSlice";
import { Favorite, FavoriteBorder } from "@mui/icons-material";
import { addLovedItemAsync, removeLovedItemAsync } from "../loved/lovedSlice";

interface Props {
  product: Product;
}

export default function ProductCard({ product }: Props) {
  const { status } = useAppSelector((state) => state.basket);
  const { loved } = useAppSelector((state) => state.loved);
  const dispatch = useAppDispatch();

  let title = product.name;

  if (title.length < 32) {
    title = `${product.name}${"\xa0".repeat(10)}`;
  }

  function handleClick() {
    if (
      loved?.items &&
      loved.items.some((item) => item.productId === product.id)
    ) {
      dispatch(removeLovedItemAsync({ productId: product.id }));
    } else {
      dispatch(addLovedItemAsync({ productId: product.id }));
    }
  }

  return (
    <Card>
      <CardMedia
        sx={{
          height: 220,
          backgroundSize: "contain",
          bgcolor: "#FFFFFF",
        }}
        component={Link}
        to={`/catalog/${product.id}`}
        image={product.pictureUrl}
        title={product.name}
      />
      <CardContent>
        <Typography
          component={Link}
          to={`/catalog/${product.id}`}
          gutterBottom
          color="secondary"
          variant="h6"
          sx={{ textDecoration: "none" }}
        >
          {title}
        </Typography>
        <Typography gutterBottom color="text.secondary" variant="h6">
          {currencyFormat(product.price)}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {product.brand} / {product.type}
        </Typography>
      </CardContent>
      <CardActions>
        <LoadingButton
          loading={status === `pendingAddItem${product.id}`}
          onClick={() => {
            dispatch(
              addBasketItemAsync({ productId: product.id, quantity: 1 })
            );
            console.log("pendingAddItem" + product.id);
          }}
          variant="contained"
          fullWidth
        >
          Add to cart
        </LoadingButton>
        <IconButton
          onClick={() => {
            handleClick();
          }}
          size="large"
          edge="start"
          color="inherit"
          sx={{ p: 2 }}
        >
          {loved?.items &&
          loved.items.some((item) => item.productId === product.id) ? (
            <Favorite />
          ) : (
            <FavoriteBorder />
          )}
        </IconButton>
      </CardActions>
    </Card>
  );
}
