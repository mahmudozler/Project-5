import re
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup
from multiprocessing import Pool


class Product(object):
    def __init__(self):
        self.page = "https://www.coolblue.nl"

    def run(self):
        pass

    def bsPage(self, page):
        Client = urlopen(page)
        page_html = Client.read()
        Client.close()

        return soup(page_html, "html5lib")

    def getPageData(self, pageNr):
        page_soup = self.bsPage(self.page + "?pagina={0}".format(pageNr + 1))
        cards = page_soup.findAll(
            "a", {"class": "product__title js-product-title"})
        card_links = [card["href"] for card in cards]

        print("\t" + self.page + "?pagina={0}".format(pageNr + 1))

        p = Pool(10)
        p.map(self.getCardData, [card_link for card_link in card_links])
        p.terminate()
        p.join()

    def getSpec(self, card_detail, spec):
        try:
            return card_detail.find(text=re.compile(spec)).parent.parent.dd.text.strip().replace("\"", "''")
        except:
            try:
                return card_detail.find(text=re.compile(spec)).parent.parent.parent.parent.dd.text.strip().replace("\"", "''")
            except:
                try:
                    return card_detail.find(text=re.compile(spec)).parent.dd.text.strip().replace("\"", "''")
                except:
                    return "None"

    def addSpec(self, specs, spec):
        specs.append("\"{0}\"".format(spec.replace("\"", "''")))

    def getCardData(self, card_link):
        pass
