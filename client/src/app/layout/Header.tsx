import {
  Brightness3,
  FavoriteBorder,
  LightMode,
  ShoppingCart,
} from "@mui/icons-material";
import {
  AppBar,
  Badge,
  Box,
  Container,
  IconButton,
  List,
  ListItem,
  Toolbar,
} from "@mui/material";
import { Link, NavLink } from "react-router-dom";
import { useAppSelector } from "../store/configureStore";
import SignedInMenu from "./SignedInMenu";
import logo from "/images/logo.png";

const midLinks = [
  { title: "catalog", path: "/catalog" },
  { title: "about", path: "/about" },
  { title: "contact", path: "/contact" },
];

const rightLinks = [
  { title: "login", path: "/login" },
  { title: "register", path: "/register" },
];

const navStyles = {
  color: "inherit",
  textDecoration: "none",
  typography: "h7",
  "&:hover": {
    color: "grey.500",
  },
  "&.active": {
    color: "text.secondary",
  },
};

interface Props {
  darkMode: boolean;
  handleThemeChange: () => void;
}

export default function Header({ darkMode, handleThemeChange }: Props) {
  const { basket } = useAppSelector((state) => state.basket);
  const { loved } = useAppSelector((state) => state.loved);
  const { user } = useAppSelector((state) => state.account);
  const basketItemCount = basket?.items.reduce(
    (sum, item) => sum + item.quantity,
    0
  );
  const lovedItemCount = loved?.items.length;

  return (
    <AppBar
      position="static"
      sx={{ background: "linear-gradient(to right, #6157FF, #EE49FD)" }}
    >
      <Container>
        <Toolbar
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
          disableGutters={true}
        >
          <List sx={{ display: "flex", alignItems: "left" }}>
            {midLinks.map(({ title, path }) => (
              <ListItem component={NavLink} to={path} key={path} sx={navStyles}>
                {title.toUpperCase()}
              </ListItem>
            ))}
          </List>
          <Box display="flex" alignItems="center">
            <Box component={NavLink} to="/" sx={{ px: 1, py: 1 }}>
              <img src={logo} alt="Logo" height="100px" />
            </Box>
          </Box>

          <Box display="flex" alignItems="right">
            <IconButton
              size="large"
              color="inherit"
              onClick={handleThemeChange}
            >
              {darkMode === false ? <Brightness3 /> : <LightMode />}
            </IconButton>
            <IconButton
              component={Link}
              to="/loved"
              size="large"
              color="inherit"
            >
              <Badge badgeContent={lovedItemCount} color="secondary">
                <FavoriteBorder />
              </Badge>
            </IconButton>

            <IconButton
              component={Link}
              to="/basket"
              size="large"
              color="inherit"
            >
              <Badge badgeContent={basketItemCount} color="secondary">
                <ShoppingCart />
              </Badge>
            </IconButton>

            {user ? (
              <SignedInMenu />
            ) : (
              <List sx={{ display: "flex" }}>
                {rightLinks.map(({ title, path }) => (
                  <ListItem
                    component={NavLink}
                    to={path}
                    key={path}
                    sx={navStyles}
                  >
                    {title.toUpperCase()}
                  </ListItem>
                ))}
              </List>
            )}
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
}
