import { IconButton, Menu, MenuItem } from "@mui/material";
import React from "react";
import { useAppDispatch } from "../store/configureStore";
import { signOut } from "../../features/account/accountSlice";
import { clearBasket } from "../../features/basket/basketSlice";
import { Link } from "react-router-dom";
import { clearLoved } from "../../features/loved/lovedSlice";
import { Person } from "@mui/icons-material";

export default function SignedInMenu() {
  const dispatch = useAppDispatch();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <>
      <IconButton size="large" color="inherit" onClick={handleClick}>
        <Person />
      </IconButton>
      <Menu anchorEl={anchorEl} open={open} onClose={handleClose}>
        <MenuItem onClick={handleClose}>Profile</MenuItem>
        <MenuItem component={Link} to="/orders">
          My orders
        </MenuItem>
        <MenuItem
          onClick={() => {
            dispatch(signOut());
            dispatch(clearBasket());
            dispatch(clearLoved());
          }}
        >
          Logout
        </MenuItem>
      </Menu>
    </>
  );
}
