import {
  Container,
  CssBaseline,
  ThemeProvider,
  createTheme,
} from "@mui/material";
import Header from "./Header";
import { useCallback, useEffect, useState } from "react";
import { Outlet, useLocation } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/ReactToastify.css";
import LoadingComponent from "./LoadingComponent";
import { useAppDispatch } from "../store/configureStore";
import { fetchBasketAsync } from "../../features/basket/basketSlice";
import { fetchCurrentUser } from "../../features/account/accountSlice";
import { fetchLovedAsync } from "../../features/loved/lovedSlice";
import Footer from "./Footer";
import HomePage from "../../features/home/HomePage";

function App() {
  const location = useLocation();
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);

  const initApp = useCallback(async () => {
    try {
      await dispatch(fetchCurrentUser());
      await dispatch(fetchBasketAsync());
      await dispatch(fetchLovedAsync());
    } catch (error) {
      console.log(error);
    }
  }, [dispatch]);

  useEffect(() => {
    initApp().then(() => setLoading(false));
  }, [initApp]);

  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? "dark" : "light";

  const theme = createTheme({
    palette: {
      primary: {
        main: "#6157FF",
      },
      secondary: {
        main: "#EE49FD",
        light: "#F5EBFF",
        contrastText: "#47008F",
      },
      mode: paletteType,
      background: {
        default: paletteType === "light" ? "#F7F6F4" : "#121212",
      },
    },
    typography: {
      fontFamily: ["Rubik", "sans-serif"].join(","),
    },
  });

  function handleThemeChange() {
    setDarkMode(!darkMode);
  }

  if (loading) return;

  return (
    <ThemeProvider theme={theme}>
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
      <CssBaseline />
      <Header darkMode={darkMode} handleThemeChange={handleThemeChange} />
      {loading ? (
        <LoadingComponent message="Initialising app..." />
      ) : location.pathname === "/" ? (
        <HomePage />
      ) : (
        <Container sx={{ mt: 4, minHeight: "100vh" }}>
          <Outlet />
        </Container>
      )}

      <Footer />
    </ThemeProvider>
  );
}

export default App;
