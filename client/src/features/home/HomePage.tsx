import { Box, Container, Divider, Grid, Typography } from "@mui/material";
import { ImageSlider } from "../../app/components/ImageSlider/ImageSlider";
import { Euro, LocalShipping, SupportAgent } from "@mui/icons-material";
import { Product } from "../../app/models/product";
import { useState, useEffect } from "react";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import ProductCardSkeleton from "../catalog/ProductCardSkeleton";
import ProductCard from "../catalog/ProductCard";

const IMAGES = [
  { url: "/images/1-slide.jpg", alt: "1" },
  { url: "/images/2-slide.jpg", alt: "2" },
  { url: "/images/3-slide.jpg", alt: "3" },
  { url: "/images/4-slide.jpg", alt: "4" },
];

export default function HomePage() {
  const [products, setProducts] = useState<Product[] | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    agent.Catalog.newest()
      .then((products) => setProducts(products))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <LoadingComponent message="Loading home page..." />;
  return (
    <div
      style={{
        maxHeight: "600px",
        width: "100%",
        aspectRatio: "10 / 6",
        margin: "0 auto",
      }}
    >
      <ImageSlider images={IMAGES} />
      <Container>
        <Grid container columnSpacing={8} sx={{ my: 10, pr: 1, pl: 1 }}>
          <Grid item xs={4}>
            <Box textAlign="center">
              <LocalShipping fontSize="large" />
              <Typography variant="h4" sx={{ my: 2 }}>
                Lorem ipsum
              </Typography>
              <Typography variant="body1">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut
                enim ad minim veniam, quis nostrud exercitation ullamco laboris
                nisi ut aliquip ex ea commodo consequat.
              </Typography>
            </Box>
          </Grid>
          <Grid item xs={4}>
            <Box textAlign="center">
              <Euro fontSize="large" />
              <Typography variant="h4" sx={{ my: 2 }}>
                Lorem ipsum
              </Typography>
              <Typography variant="body1">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut
                enim ad minim veniam, quis nostrud exercitation ullamco laboris
                nisi ut aliquip ex ea commodo consequat.
              </Typography>
            </Box>
          </Grid>
          <Grid item xs={4}>
            <Box textAlign="center">
              <SupportAgent fontSize="large" />
              <Typography variant="h4" sx={{ my: 2 }}>
                Lorem ipsum
              </Typography>
              <Typography variant="body1">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut
                enim ad minim veniam, quis nostrud exercitation ullamco laboris
                nisi ut aliquip ex ea commodo consequat.
              </Typography>
            </Box>
          </Grid>
        </Grid>
        <Box sx={{ my: 10 }}>
          <Typography textAlign="center" variant="h4">
            Newest products{" "}
          </Typography>
          <Divider sx={{ my: 4 }} />
          <Grid item xs={12}>
            <Grid container spacing={4}>
              {products &&
                products.map((product) => (
                  <Grid item xs={4} key={product.id}>
                    {loading ? (
                      <ProductCardSkeleton />
                    ) : (
                      <ProductCard product={product} />
                    )}
                  </Grid>
                ))}
            </Grid>
          </Grid>
        </Box>
      </Container>
    </div>
  );
}
