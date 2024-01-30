import { Facebook, Instagram, Pinterest } from "@mui/icons-material";
import {
  Box,
  Container,
  Divider,
  List,
  ListItem,
  Typography,
} from "@mui/material";
import { NavLink } from "react-router-dom";
import logo from "/images/logo.png";

const links = [
  { title: "home", path: "/" },
  { title: "about", path: "/about" },
  { title: "contact", path: "/contact" },
];

const navStyles = {
  color: "white",
  textDecoration: "none",
  typography: "h7",
  "&:hover": {
    color: "grey.500",
  },
  "&.active": {
    color: "text.secondary",
  },
};

export default function Footer() {
  return (
    <Box
      component="footer"
      sx={{
        background: "linear-gradient(to right, #6157FF, #EE49FD)",
        marginTop: "auto",
      }}
    >
      <Container>
        <Box display="flex" alignItems="center" justifyContent="space-between">
          <Box component={NavLink} to="/" sx={{ px: 1, py: 1 }}>
            <img src={logo} alt="Logo" height="100px" />
          </Box>
          <List sx={{ display: "flex", alignItems: "left" }}>
            {links.map(({ title, path }) => (
              <ListItem component={NavLink} to={path} key={path} sx={navStyles}>
                {title.toUpperCase()}
              </ListItem>
            ))}
          </List>
        </Box>
        <Divider />
        <Box
          display="flex"
          alignItems="center"
          justifyContent="space-between"
          sx={{ py: 3 }}
        >
          <Typography color="white" variant="overline">
            © 2024 MasterType. All rights reserved.
          </Typography>
          <Box color="white">
            <Instagram sx={{ mr: 1 }} />
            <Facebook sx={{ mr: 1 }} />
            <Pinterest sx={{ mr: 1 }} />
          </Box>
        </Box>
      </Container>
    </Box>
  );
}
