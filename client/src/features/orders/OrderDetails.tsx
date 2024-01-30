import {
  Box,
  Chip,
  Grid,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Typography,
} from "@mui/material";
import BasketTable from "../basket/BasketTable";
import { useEffect, useState } from "react";
import { Order } from "../../app/models/order";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { currencyFormat } from "../../app/util/util";
import { BasketItem } from "../../app/models/basket";
import { useParams } from "react-router-dom";

export default function OrderDetails() {
  const { id } = useParams<{ id: string }>();
  const [order, setOrder] = useState<Order | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (id) {
      agent.Orders.fetch(parseInt(id))
        .then((order) => setOrder(order))
        .catch((error) => console.log(error))
        .finally(() => setLoading(false));
    }
  }, [id]);

  if (loading) return <LoadingComponent message="Loading order details..." />;

  return (
    <>
      {order && (
        <>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 1,
            }}
          >
            <Typography variant="h4">Order id: {order.id}</Typography>
            <Chip label={"Status: " + order.orderStatus} color="primary" />
          </Box>
          <Typography
            variant="h5"
            sx={{
              mb: 1,
            }}
          >
            Date: {order.orderDate.split("T")[0]}
          </Typography>
          <BasketTable
            items={order?.orderItems as BasketItem[]}
            isBasket={false}
          />
          <Grid container>
            <Grid item xs={6} />
            <Grid item xs={6}>
              <TableContainer component={Paper} variant={"outlined"}>
                <Table>
                  <TableBody>
                    <TableRow>
                      <TableCell colSpan={2}>Subtotal</TableCell>
                      <TableCell align="right">
                        {currencyFormat(order?.subtotal)}
                      </TableCell>
                    </TableRow>
                    <TableRow>
                      <TableCell colSpan={2}>Delivery fee*</TableCell>
                      <TableCell align="right">
                        {currencyFormat(order?.deliveryFee)}
                      </TableCell>
                    </TableRow>
                    <TableRow>
                      <TableCell colSpan={2}>Total</TableCell>
                      <TableCell align="right">
                        {currencyFormat(order?.total)}
                      </TableCell>
                    </TableRow>
                  </TableBody>
                </Table>
              </TableContainer>
            </Grid>
          </Grid>
        </>
      )}
    </>
  );
}
