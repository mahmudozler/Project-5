import re
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup
from multiprocessing import Pool

import Product


class Speaker(Product.Product):
    def __init__(self):
        self.page = "https://www.coolblue.nl/producttype:speakers"

    def run(self):
        headers = "product_type,product_name,product_desc,product_price,product_brand,product_model,product_speakertype,product_color,product_waterres,product_filter,product_freq,product_imp,product_dimensions,product_weight"
        page_count = self.start("Data/product_speakers.csv", headers)

        for i in range(page_count):
            self.getPageData(i)

    def getCardData(self, card_link):
        if len(card_link[2]) < 16:
            card_detail = self.bsPage(self.page[:23] + card_link[2])
            product_specs = [card_link[1]]

            self.addSpec(product_specs,
                         card_detail.h1.text.strip())
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "", True))
            self.addSpec(product_specs,
                         card_detail.find("strong", {"class": "sales-price--current"}).text.strip())
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Merk"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Fabrikantcode"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Type speaker"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Kleur"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Waterbestendig")
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Filter"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Frequentiebereik"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Impedantie"))
            self.addSpec(product_specs, "{0} x {1} x {2}".format(
                self.getSpec(card_detail, "Breedte"), self.getSpec(card_detail, "Diepte"), self.getSpec(card_detail, "Hoogte")))
            self.addSpec(product_specs, self.getSpec(
                card_detail, "Gewicht"))

            filename = "Data/product_speakers.csv"
            file = open(filename, "a")
            file.write(",".join(product_specs) + "\n")
            file.close()

            self.getImages(card_detail, product_specs)

            print(product_specs[0] + "\tproduct_name: " + product_specs[1])
