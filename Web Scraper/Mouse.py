import re
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup
from multiprocessing import Pool

import Product


class Mouse(Product.Product):
    def __init__(self):
        self.page = "https://www.coolblue.nl/producttype:muizen,gaming-muizen"

    def run(self):
        headers = "product_type,product_name,product_price,product_brand,product_model,product_mousemodel,product_lr,product_buttons,product_sensor,product_dpi,product_connection,product_os,product_dimensions,product_color"
        page_count = self.start("Data/product_mouse.csv", headers)

        for i in range(page_count):
            self.getPageData(i)

    def getCardData(self, card_link):
        if len(card_link) < 16:
            card_detail = self.bsPage(self.page[:23] + card_link[2])
            product_specs = [card_link[1]]

            self.addSpec(product_specs,
                         card_detail.h1.text.strip())
            self.addSpec(product_specs,
                         card_detail.find("strong", {"class": "sales-price--current"}).text.strip())
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Merk"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Fabrikantcode"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Model muis"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Links- of rechtshandig"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Aantal knoppen"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Aansturing"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Dots per inch (dpi)"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Verbinding"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Geschikt voor"))
            self.addSpec(product_specs, "{0} x {1} x {2}".format(
                self.getSpec(card_detail, "Breedte"), self.getSpec(card_detail, "Diepte"), self.getSpec(card_detail, "Hoogte")))
            self.addSpec(product_specs, self.getSpec(
                card_detail, "Kleur"))

            filename = "Data/product_mouse.csv"
            file = open(filename, "a")
            file.write(",".join(product_specs) + "\n")
            file.close()

            self.getImages(card_detail, product_specs)

            print(product_specs[0] + "\tproduct_name: " + product_specs[1])
