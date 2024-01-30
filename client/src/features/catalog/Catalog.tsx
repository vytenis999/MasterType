import LoadingComponent from "../../app/layout/LoadingComponent";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import ProductList from "./ProductList";
import { useEffect } from "react";
import {
  fetchFilters,
  fetchProductsAsync,
  productSelectors,
  setPageNumber,
  setProductParams,
} from "./catalogSlice";
import { Grid, Paper } from "@mui/material";
import ProductSearch from "./ProductSearch";
import AppPagination from "../../app/components/AppPagination";
import AppSelectBox from "../../app/components/AppSelectBox";
import AppMultipleSelectBox from "../../app/components/AppMultipleSelectBox";

const sortOptions = [
  { value: "name", label: "Alphabetical" },
  { value: "priceDesc", label: "Price - High to low" },
  { value: "price", label: "Price - Low to high" },
];

export default function Catalog() {
  const products = useAppSelector(productSelectors.selectAll);
  const {
    productsLoaded,
    filtersLoaded,
    brands,
    types,
    productParams,
    metaData,
  } = useAppSelector((state) => state.catalog);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (!productsLoaded) dispatch(fetchProductsAsync());
  }, [productsLoaded, dispatch]);

  useEffect(() => {
    if (!filtersLoaded) dispatch(fetchFilters());
  }, [filtersLoaded, dispatch]);

  if (!filtersLoaded) return <LoadingComponent message="Loading products..." />;

  return (
    <Grid container rowSpacing={4} columnSpacing={4} sx={{ pr: 1, pl: 1 }}>
      <Grid item xs={3}>
        <Paper sx={{ mb: 2 }}>
          <ProductSearch />
        </Paper>
      </Grid>
      <Grid item xs={3}>
        <Paper sx={{ mb: 2 }}>
          <AppSelectBox
            label="Sorting"
            selectedValue={productParams.orderBy}
            options={sortOptions}
            onChange={(event) =>
              dispatch(setProductParams({ orderBy: event.target.value }))
            }
          />
        </Paper>
      </Grid>
      <Grid item xs={3}>
        <Paper sx={{ mb: 2 }}>
          <AppMultipleSelectBox
            items={brands}
            checked={productParams.brands}
            onChange={(items: string[]) =>
              dispatch(setProductParams({ brands: items }))
            }
            label="Brands"
          />
        </Paper>
      </Grid>
      <Grid item xs={3}>
        <Paper sx={{ mb: 2 }}>
          <AppMultipleSelectBox
            items={types}
            checked={productParams.types}
            onChange={(items: string[]) =>
              dispatch(setProductParams({ types: items }))
            }
            label="Categories"
          />
        </Paper>
      </Grid>
      <Grid item xs={12}>
        <ProductList products={products} />
      </Grid>
      <Grid item xs={12} sx={{ mt: 3, mb: 3 }}>
        {metaData && (
          <AppPagination
            metaData={metaData}
            onPageChange={(page: number) =>
              dispatch(setPageNumber({ pageNumber: page }))
            }
          />
        )}
      </Grid>
    </Grid>
  );
}
